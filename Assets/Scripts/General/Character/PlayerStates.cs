


namespace ms
{
	// Base class for player states
	public abstract class PlayerState : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

		// Actions taken when transitioning into the state
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void initialize(Player& player) const = 0;
		public abstract void initialize (Player player);

		// How to handle inputs while in the state
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void send_action(Player& player, KeyAction::Id action, bool pressed) const = 0;
		public abstract void send_action (Player player, KeyAction.Id action, bool pressed);

		// Actions taken in the player's update method, before physics are applied.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void update(Player& player) const = 0;
		public abstract void update (Player player);

		// Transition into a new state after physics have been applied
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void update_state(Player& player) const = 0;
		public abstract void update_state (Player player);

		// Play the jumping sound
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void play_jumpsound() const
		protected void play_jumpsound ()
		{
			new Sound(Sound.Name.JUMP).play();
		}

		// Check if the left or right key is pressed
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool haswalkinput(const Player& player) const
		protected bool haswalkinput (Player player)
		{
			return player.is_key_down (KeyAction.Id.LEFT) || player.is_key_down (KeyAction.Id.RIGHT);
		}

		// Check if only the left key is pressed and not the right key
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool hasleftinput(const Player& player) const
		protected bool hasleftinput (Player player)
		{
			return player.is_key_down (KeyAction.Id.LEFT) && !player.is_key_down (KeyAction.Id.RIGHT);
		}

		// Check if only the right key is pressed and not the left key
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool hasrightinput(const Player& player) const
		protected bool hasrightinput (Player player)
		{
			return player.is_key_down (KeyAction.Id.RIGHT) && !player.is_key_down (KeyAction.Id.LEFT);
		}
	}

	// The initial state, determines which state the player should be in.
	public class PlayerNullState : PlayerState
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void initialize(Player&) const override
		public override void initialize (Player UnnamedParameter1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void send_action(Player&, KeyAction::Id, bool) const override
		public override void send_action (Player UnnamedParameter1, KeyAction.Id UnnamedParameter2, bool UnnamedParameter3)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update(Player&) const override
		public override void update (Player UnnamedParameter1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update_state(Player& player) const override
		public override void update_state (Player player)
		{
			Char.State state = new Char.State ();

			if (player.get_phobj ().onground)
			{
				if (player.is_key_down (KeyAction.Id.LEFT))
				{
					state = Char.State.WALK;

					player.set_direction (false);
				}
				else if (player.is_key_down (KeyAction.Id.RIGHT))
				{
					state = Char.State.WALK;

					player.set_direction (true);
				}
				else if (player.is_key_down (KeyAction.Id.DOWN))
				{
					state = Char.State.PRONE;
				}
				else
				{
					state = Char.State.STAND;
				}
			}
			else
			{
				var ladder = player.get_ladder ();

				if (ladder)
				{
					state = ladder.get ().is_ladder () ? Char.State.LADDER : Char.State.ROPE;
				}
				else
				{
					state = Char.State.FALL;
				}
			}

			player.get_phobj ().clear_flags ();
			player.set_state (state);
		}
	}

	// The standing state
	public class PlayerStandState : PlayerState
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void initialize(Player& player) const override
		public override void initialize (Player player)
		{
			player.get_phobj ().type = PhysicsObject.Type.NORMAL;
			//if(!haswalkinput (player)) player.get_phobj ().hspeed = 0;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void send_action(Player& player, KeyAction::Id ka, bool down) const override
		public override void send_action (Player player, KeyAction.Id ka, bool down)
		{
			if (player.is_attacking ())
			{
				return;
			}

			if (down && ka == KeyAction.Id.JUMP)
			{
				play_jumpsound ();

				player.get_phobj ().vforce = -player.get_jumpforce ();
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update(Player& player) const override
		public override void update (Player player)
		{
			if (player.get_phobj ().enablejd == false)
			{
				player.get_phobj ().set_flag (PhysicsObject.Flag.CHECKBELOW);
			}

			if (player.is_attacking ())
			{
				return;
			}

			if (hasrightinput (player))
			{
				player.set_direction (true);
				player.set_state (Char.State.WALK);
			}
			else if (hasleftinput (player))
			{
				player.set_direction (false);
				player.set_state (Char.State.WALK);
			}

			if (player.is_key_down (KeyAction.Id.DOWN) && !player.is_key_down (KeyAction.Id.UP) && !haswalkinput (player))
			{
				player.set_state (Char.State.PRONE);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update_state(Player& player) const override
		public override void update_state (Player player)
		{
			if (!player.get_phobj ().onground)
			{
				player.set_state (Char.State.FALL);
			}
		}
	}

	// The walking state
	public class PlayerWalkState : PlayerState
	{
		public override void initialize (Player player)
		{
			player.get_phobj ().type = PhysicsObject.Type.NORMAL;
		}

		public override void send_action (Player player, KeyAction.Id ka, bool down)
		{
			if (player.is_attacking ())
			{
				return;
			}

			if (down && ka == KeyAction.Id.JUMP)
			{
				play_jumpsound ();

				player.get_phobj ().vforce = -player.get_jumpforce ();
			}

			if (down && ka == KeyAction.Id.JUMP && player.is_key_down (KeyAction.Id.DOWN) && player.get_phobj ().enablejd)
			{
				play_jumpsound ();

				player.get_phobj ().y = player.get_phobj ().groundbelow;
				player.set_state (Char.State.FALL);
			}
		}

		public override void update (Player player)
		{
			if (player.get_phobj ().enablejd == false)
			{
				player.get_phobj ().set_flag (PhysicsObject.Flag.CHECKBELOW);
			}

			if (player.is_attacking ())
			{
				return;
			}

			if (haswalkinput (player))
			{
				if (hasrightinput (player))
				{
					player.set_direction (true);
					player.get_phobj ().hforce += player.get_walkforce ();
				}
				else if (hasleftinput (player))
				{
					player.set_direction (false);
					player.get_phobj ().hforce += -player.get_walkforce ();
				}
			}
			else
			{
				if (player.is_key_down (KeyAction.Id.DOWN))
				{
					player.set_state (Char.State.PRONE);
				}
			}
		}

		public override void update_state (Player player)
		{
			if (player.get_phobj ().onground)
			{
				if (!haswalkinput (player) || player.get_phobj ().hspeed == 0.0f)
				{
					player.set_state (Char.State.STAND);
				}
			}
			else
			{
				player.set_state (Char.State.FALL);
			}
		}
	}

	// The falling state
	public class PlayerFallState : PlayerState
	{
		public override void initialize (Player player)
		{
			player.get_phobj ().type = PhysicsObject.Type.NORMAL;
		}

		public override void send_action (Player player, KeyAction.Id ka, bool down)
		{
		}

		public override void update (Player player)
		{
			if (player.is_attacking ())
			{
				return;
			}

			var hspeed = player.get_phobj ().hspeed;

			if (hasleftinput (player) && hspeed > 0.0) //todo 2 where is real move speed???
			{
				hspeed -= 0.025;
			}
			else if (hasrightinput (player) && hspeed < 0.0)
			{
				hspeed += 0.025;
			}

			if (hasleftinput (player))
			{
				player.set_direction (false);
			}
			else if (hasrightinput (player))
			{
				player.set_direction (true);
			}
		}

		public override void update_state (Player player)
		{
			if (player.get_phobj ().onground)
			{
				if (player.is_key_down (KeyAction.Id.DOWN) && !haswalkinput (player))
				{
					player.set_state (Char.State.PRONE);
				}
				else
				{
					player.set_state (Char.State.STAND);
				}
			}
			else if (player.is_underwater ())
			{
				player.set_state (Char.State.SWIM);
			}
		}
	}

	// The prone state (Lying down)
	public class PlayerProneState : PlayerState
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void initialize(Player&) const override
		public override void initialize (Player UnnamedParameter1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void send_action(Player& player, KeyAction::Id ka, bool down) const override
		public override void send_action (Player player, KeyAction.Id ka, bool down)
		{
			if (down && ka == KeyAction.Id.JUMP)
			{
				if (player.get_phobj ().enablejd && player.is_key_down (KeyAction.Id.DOWN))
				{
					play_jumpsound ();

					player.get_phobj ().y = player.get_phobj ().groundbelow;
					player.set_state (Char.State.FALL);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update(Player& player) const override
		public override void update (Player player)
		{
			if (player.get_phobj ().enablejd == false)
			{
				player.get_phobj ().set_flag (PhysicsObject.Flag.CHECKBELOW);
			}

			if (player.is_key_down (KeyAction.Id.UP) || !player.is_key_down (KeyAction.Id.DOWN))
			{
				player.set_state (Char.State.STAND);
			}

			if (player.is_key_down (KeyAction.Id.LEFT))
			{
				player.set_direction (false);
				player.set_state (Char.State.WALK);
			}

			if (player.is_key_down (KeyAction.Id.RIGHT))
			{
				player.set_direction (true);
				player.set_state (Char.State.WALK);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update_state(Player&) const override
		public override void update_state (Player UnnamedParameter1)
		{
		}
	}

	// The sitting state
	public class PlayerSitState : PlayerState
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void initialize(Player&) const override
		public override void initialize (Player UnnamedParameter1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void send_action(Player& player, KeyAction::Id ka, bool down) const override
		public override void send_action (Player player, KeyAction.Id ka, bool down)
		{
			if (down)
			{
				switch (ka)
				{
					case KeyAction.Id.LEFT:
						player.set_direction (false);
						player.set_state (Char.State.WALK);
						break;
					case KeyAction.Id.RIGHT:
						player.set_direction (true);
						player.set_state (Char.State.WALK);
						break;
					case KeyAction.Id.JUMP:
						player.set_state (Char.State.STAND);
						break;
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update(Player&) const override
		public override void update (Player UnnamedParameter1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update_state(Player&) const override
		public override void update_state (Player UnnamedParameter1)
		{
		}
	}

	// The flying or swimming state
	public class PlayerFlyState : PlayerState
	{
		public override void initialize (Player player)
		{
			player.get_phobj ().type = player.is_underwater () ? PhysicsObject.Type.SWIMMING : PhysicsObject.Type.FLYING;
		}

		public override void send_action (Player player, KeyAction.Id ka, bool down)
		{
			if (down)
			{
				switch (ka)
				{
					case KeyAction.Id.LEFT:
						player.set_direction (false);
						break;
					case KeyAction.Id.RIGHT:
						player.set_direction (true);
						break;
				}
			}
		}

		public override void update (Player player)
		{
			if (player.is_attacking ())
			{
				return;
			}

			if (player.is_key_down (KeyAction.Id.LEFT))
			{
				player.get_phobj ().hforce = -player.get_flyforce ();
			}
			else if (player.is_key_down (KeyAction.Id.RIGHT))
			{
				player.get_phobj ().hforce = player.get_flyforce ();
			}

			if (player.is_key_down (KeyAction.Id.UP))
			{
				player.get_phobj ().vforce = -player.get_flyforce ();
			}
			else if (player.is_key_down (KeyAction.Id.DOWN))
			{
				player.get_phobj ().vforce = player.get_flyforce ();
			}
		}

		public override void update_state (Player player)
		{
			if (player.get_phobj ().onground && player.is_underwater ())
			{
				Char.State state = new Char.State ();

				if (player.is_key_down (KeyAction.Id.LEFT))
				{
					state = Char.State.WALK;

					player.set_direction (false);
				}
				else if (player.is_key_down (KeyAction.Id.RIGHT))
				{
					state = Char.State.WALK;

					player.set_direction (true);
				}
				else if (player.is_key_down (KeyAction.Id.DOWN))
				{
					state = Char.State.PRONE;
				}
				else
				{
					state = Char.State.STAND;
				}

				player.set_state (state);
			}
		}
	}

	// The climbing state
	public class PlayerClimbState : PlayerState
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void initialize(Player& player) const override
		public override void initialize (Player player)
		{
			player.get_phobj ().type = PhysicsObject.Type.FIXATED;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void send_action(Player&, KeyAction::Id, bool) const override
		public override void send_action (Player UnnamedParameter1, KeyAction.Id UnnamedParameter2, bool UnnamedParameter3)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update(Player& player) const override
		public override void update (Player player)
		{
			if (player.is_key_down (KeyAction.Id.UP) && !player.is_key_down (KeyAction.Id.DOWN))
			{
				player.get_phobj ().vspeed = -player.get_climbforce ();
			}
			else if (player.is_key_down (KeyAction.Id.DOWN) && !player.is_key_down (KeyAction.Id.UP))
			{
				player.get_phobj ().vspeed = player.get_climbforce ();
			}
			else
			{
				player.get_phobj ().vspeed = 0.0;
			}

			if (player.is_key_down (KeyAction.Id.JUMP) && haswalkinput (player))
			{
				play_jumpsound ();

				var walkforce = player.get_walkforce () * 8.0;

				player.set_direction (player.is_key_down (KeyAction.Id.RIGHT));

				player.get_phobj ().hspeed = player.is_key_down (KeyAction.Id.LEFT) ? -walkforce : walkforce;
				player.get_phobj ().vspeed = -player.get_jumpforce () / 1.5;

				cancel_ladder (player);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void update_state(Player& player) const override
		public override void update_state (Player player)
		{
			short y = player.get_phobj ().get_y ();
			bool downwards = player.is_key_down (KeyAction.Id.DOWN);
			var ladder = player.get_ladder ();

			if (ladder && ladder.get ().felloff (y, downwards))
			{
				cancel_ladder (player);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void cancel_ladder(Player& player) const
		private void cancel_ladder (Player player)
		{
			player.set_state (Char.State.FALL);
			player.set_ladder (null);
			player.set_climb_cooldown ();
		}
	}
}

namespace ms
{
	#region Base

	#endregion

	#region Null

	#endregion

	#region Standing

	#endregion

	#region Walking

	#endregion

	#region Falling

	#endregion

	#region Prone

	#endregion

	#region Sitting

	#endregion

	#region Flying

	#endregion

	#region Climbing

	#endregion
}