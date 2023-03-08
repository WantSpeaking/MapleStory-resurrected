set WORKSPACE=G:\GithubProjects\\FS_Config

set GEN_CLIENT=%WORKSPACE%\Tools\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=%WORKSPACE%\DesignerConfigs

%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir G:\Unity-Projects\ForeverStory\Assets\Scripts\GenCodes ^
 --output_data_dir G:\Unity-Projects\ForeverStory\Assets\Resources\GenerateDatas ^
 --gen_types code_cs_unity_json,data_json ^
 -s all 

pause