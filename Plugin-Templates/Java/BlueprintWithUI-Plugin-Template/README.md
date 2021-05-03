### BlueprintWithUI-Plugin-Template

This is a template application for plugins of type `Run as Blueprint - with UI`

---

#### Notes for development:
- Create or develop the logic of your application in `PluginExecutor.java` (in `com.cognizant.blueprintwithui` package)
- Modify the `ui.json` file under `StartUpFiles` directory, as per your requirements.
    - Refer to the Plugin's Help document for details on how to configure this file.
- Modify the `PluginManifest.yml` file under `StartUpFiles` directory, as per your requirements.
- Modify the `run.bat` and `run.sh` file under `StartUpFiles` directory, as per your requirements.
- Create a file named `help.md` or `help.txt` under `StartUpFiles` directory, if you want to provide any help contents to the user.

---

#### For creating the plugin zip file in local:
- In PluginManifest.yml file
    - Add `pluginSource` as `Custom`
    - Add `pluginId` as some unique value like `upshift.blueprint.ui.template`
- The maven's `package` task will create a zip file with the application jar file along with the `StartUpFiles` folder contents 
(application jar, plugin manifest file, ui.json file, batch script and shell script)
    - If you want to add any file to the plugin zip, you can just add it to the `StartUpFiles` folder.
- Run the maven's `package` task and get the plugin's zip from `target` folder, and use it with UpShift.

---

#### Note
- Kindly contact UpShift team, if you have any suggestions to be done in this template application, that might be helpful for others.

---
