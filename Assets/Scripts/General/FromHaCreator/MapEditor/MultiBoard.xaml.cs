/* Copyright (C) 2015 haha01haha01
 * 2020 lastbattle

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

// uncomment line below to use XNA's Z-order functions
// #define UseXNAZorder

// uncomment line below to show FPS counter
// #define FPS_TEST


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;

using HaCreator.Collections;
//using HaCreator.MapEditor.Input;
using HaCreator.MapEditor.Instance;
//using HaCreator.MapEditor.Text;
using HaCreator.MapSimulator;
//using HaSharedLibrary.Util;
using MapleLib.WzLib.WzStructure.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;



namespace HaCreator.MapEditor
{
    public partial class MultiBoard 
    {
        private GraphicsDevice DxDevice;
        /// <summary>
        /// Gets the graphic device for rendering on the multiboard
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
	        get { return DxDevice; }
        }

        private Microsoft.Xna.Framework.Graphics.SpriteBatch sprite;
        private readonly PresentationParameters pParams = new PresentationParameters();
        private Microsoft.Xna.Framework.Graphics.Texture2D pixel;

        //private FontEngine fontEngine;
        private Thread renderer;
        private bool needsReset = false;
        private readonly IntPtr dxHandle;
        //private readonly UserObjectsManager userObjs;
        //private Scheduler scheduler;

        // UI
        private readonly List<Board> boards = new List<Board>();
        public List<Board> Boards
        {
	        get
	        {
		        return boards;
	        }
        }
        public Board SelectedBoard
        {
	        get
	        {
		        return selectedBoard;
	        }
	        set
	        {
		        lock (this)
		        {
			        selectedBoard = value;
		        }
	        }
        }
        private Board selectedBoard = null;


        #region Events
        public delegate void UndoRedoDelegate ();
        public event UndoRedoDelegate OnUndoListChanged;
        public event UndoRedoDelegate OnRedoListChanged;

        public delegate void LayerTSChangedDelegate (Layer layer);
        public event LayerTSChangedDelegate OnLayerTSChanged;

        public delegate void MenuItemClickedDelegate (BoardItem item);
        public event MenuItemClickedDelegate OnEditInstanceClicked;
        public event MenuItemClickedDelegate OnEditBaseClicked;
        public event MenuItemClickedDelegate OnSendToBackClicked;
        public event MenuItemClickedDelegate OnBringToFrontClicked;

        public delegate void ReturnToSelectionStateDelegate ();
        public event ReturnToSelectionStateDelegate ReturnToSelectionState;

        public delegate void SelectedItemChangedDelegate (BoardItem selectedItem);
        public event SelectedItemChangedDelegate SelectedItemChanged;

        public event EventHandler BoardRemoved;
        public event EventHandler<bool> MinimapStateChanged;

        public void OnSelectedItemChanged (BoardItem selectedItem)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged.Invoke (selectedItem);
        }

        public void InvokeReturnToSelectionState ()
        {
            if (ReturnToSelectionState != null)
                ReturnToSelectionState.Invoke ();
        }

        public void SendToBackClicked (BoardItem item)
        {
            if (OnSendToBackClicked != null)
                OnSendToBackClicked.Invoke (item);
        }

        public void BringToFrontClicked (BoardItem item)
        {
            if (OnBringToFrontClicked != null)
                OnBringToFrontClicked.Invoke (item);
        }

        public void EditInstanceClicked (BoardItem item)
        {
            if (OnEditInstanceClicked != null)
                OnEditInstanceClicked.Invoke (item);
        }

        public void EditBaseClicked (BoardItem item)
        {
            if (OnEditBaseClicked != null)
                OnEditBaseClicked.Invoke (item);
        }

        public void LayerTSChanged (Layer layer)
        {
            if (OnLayerTSChanged != null)
                OnLayerTSChanged.Invoke (layer);
        }

        public void UndoListChanged ()
        {
            if (OnUndoListChanged != null)
                OnUndoListChanged.Invoke ();
        }

        public void RedoListChanged ()
        {
            if (OnRedoListChanged != null)
                OnRedoListChanged.Invoke ();
        }
        #endregion

        #region Static Settings
        public static float FirstSnapVerification;
        public static Color InactiveColor;
        public static Color RopeInactiveColor;
        public static Color FootholdInactiveColor;
        public static Color ChairInactiveColor;
        public static Color ToolTipInactiveColor;
        public static Color MiscInactiveColor;
        public static Color VRInactiveColor;
        public static Color MinimapBoundInactiveColor;

        static MultiBoard ()
        {
	        RecalculateSettings ();
        }

        public static Color CreateTransparency (Color orgColor, int alpha)
        {
	        return new Color (orgColor.R, orgColor.B, orgColor.G, alpha);
        }

        public static void RecalculateSettings ()
        {
	        int alpha = UserSettings.NonActiveAlpha;
	        FirstSnapVerification = UserSettings.SnapDistance * 20;
	        InactiveColor = CreateTransparency (Color.White, alpha);
	        RopeInactiveColor = CreateTransparency (UserSettings.RopeColor, alpha);
	        FootholdInactiveColor = CreateTransparency (UserSettings.FootholdColor, alpha);
	        ChairInactiveColor = CreateTransparency (UserSettings.ChairColor, alpha);
	        ToolTipInactiveColor = CreateTransparency (UserSettings.ToolTipColor, alpha);
	        MiscInactiveColor = CreateTransparency (UserSettings.MiscColor, alpha);
	        VRInactiveColor = CreateTransparency (UserSettings.VRColor, alpha);
	        MinimapBoundInactiveColor = CreateTransparency (UserSettings.MinimapBoundColor, alpha);
        }
        #endregion

        public Board CreateBoard (Microsoft.Xna.Framework.Point mapSize, Point centerPoint)
        {
	        lock (this)
	        {
		        Board newBoard = new Board (mapSize, centerPoint, this, ApplicationSettings.theoreticalVisibleTypes, ApplicationSettings.theoreticalEditedTypes);
		        boards.Add (newBoard);
		        newBoard.CreateMapLayers ();
		        return newBoard;
	        }
        }

        public void DrawLine (SpriteBatch sprite, Vector2 start, Vector2 end, Color color)
        {
	        int width = (int)Vector2.Distance (start, end);
	        float rotation = (float)Math.Atan2 ((double)(end.Y - start.Y), (double)(end.X - start.X));
	        sprite.Draw (pixel, new Rectangle ((int)start.X, (int)start.Y, width, UserSettings.LineWidth), null, color, rotation, new Vector2 (0f, 0f), SpriteEffects.None, 1f);
        }

        public void DrawRectangle (SpriteBatch sprite, Rectangle rectangle, Color color)
        {
	        //clockwise
	        Vector2 pt1 = new Vector2 (rectangle.Left, rectangle.Top);
	        Vector2 pt2 = new Vector2 (rectangle.Right, rectangle.Top);
	        Vector2 pt3 = new Vector2 (rectangle.Right, rectangle.Bottom);
	        Vector2 pt4 = new Vector2 (rectangle.Left, rectangle.Bottom);

	        DrawLine (sprite, pt1, pt2, color);
	        DrawLine (sprite, pt2, pt3, color);
	        DrawLine (sprite, pt3, pt4, color);
	        DrawLine (sprite, pt4, pt1, color);
        }

        public void FillRectangle (SpriteBatch sprite, Rectangle rectangle, Color color)
        {
	        sprite.Draw (pixel, rectangle, color);
        }

        public void DrawDot (SpriteBatch sprite, int x, int y, Color color, int dotSize)
        {
	        int dotW = UserSettings.DotWidth * dotSize;
	        FillRectangle (sprite, new Rectangle (x - dotW, y - dotW, dotW * 2, dotW * 2), color);
        }
    }
}
