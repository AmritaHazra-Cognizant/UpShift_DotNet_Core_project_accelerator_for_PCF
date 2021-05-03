### Custom Blueprint Plugin Template

This is a template application for plugins of type `Run as Blueprint`

---

#### Notes for development:
- Check the `TODO` items in this template application, for extending it for your plugin features.
- For data persistence feature, use the methods in `com.cognizant.blueprinttemplate.service.persistence.DataPersistenceService` class.
- For file persistence feature, use the methods in `com.cognizant.blueprinttemplate.service.persistence.FilePersistenceService` class.
- Modify the `PluginManifest.yml` file under `StartUpFiles` directory, as per your requirements.
- Modify the `Start.bat` and `Start.sh` file under `StartUpFiles` directory, as per your requirements.
- Create a file named `help.md` or `help.txt` under `StartUpFiles` directory, if you want to provide any help contents to the user.

---

#### For running/testing the plugin frontend/backend during development (without UpShift)
- Modify the value returned by the method `getBackendUrl()`,
    in class `ui/src/app/common-services/common.service.ts`,
    to `http://localhost:9500`
- Run `npm start` in ui
- Run backend in `9500` port
- You might have to make few changes in code temporarily (Few obvious examples given below)
    - If UpShift is not running, then,
        Comment out `initiateResponseToUpShift()` method call,
        in class `com.cognizant.blueprinttemplate.controller.PluginController`,
        in method `processPlugin()`
    - If there are no static content in resources, then,
        Comment out the Entire class `com/cognizant/blueprinttemplate/webconfiguration/WebConfiguration.java`
        - Since we won't be using the static content before we finish developing UI
- Now, you should be able to check the UI in `http://localhost:4200` and it will be targeting the backend URL `http://localhost:9500`.

---

#### For running/testing the plugin backend during development (with UpShift)
- Run `ng build --prod` in ui, and get the static contents which will be available in `dist` folder
- Copy those static contents to `/src/main/resources/static`
- Navigate to `UpShift -> Settings -> Plugins` and click on `Register plugin using running URL`
- Register the plugin
    - URL - `http://localhost:9500`.
        - Since, `server.port` is set as 9500 in `application.properties` file
    - Relative Path to Landing Page - `home`
        - As per this plugin template
    - Relative Path to Result Page - `result`
        - As per this plugin template
- Once registered, get the details of the api key and api url from plugins settings page by hovering over the plugin card
- Make sure that `upshiftUrl` environment variable is set correctly in `application.properties` file, w.r.t the value noted above
- Set the `plugins_api_key` environment variable with the value noted before
- Start the plugin application
- Now, you should be able to navigate to the `Blueprints` page and use the plugin through UpShift.

---

#### For creating the plugin zip file in local:
- In PluginManifest.yml file
    - Add `pluginSource` as `Custom`
    - Add `pluginId` as some unique value like `upshift.blueprint.template`
- The maven `package` task will create a zip file with application jar, plugin manifest file, batch script and shell script.
    - The UI components have to be copied manually to `src/main/resources/static` before running the `package` task.
- Run the maven's `package` task and get the plugin's zip from `target` folder, and use it with UpShift.

---

#### For running/testing the plugin end to end in local
- Navigate to `UpShift -> Settings -> Plugins` and click on `Upload Plugin`
- Upload the zip file which was got by following the steps in previous section
- Navigate to the Blueprints page and test your plugin

---

#### Note
- Kindly contact UpShift team, if you have any suggestions to be done in this template application, that might be helpful for others.

---
 