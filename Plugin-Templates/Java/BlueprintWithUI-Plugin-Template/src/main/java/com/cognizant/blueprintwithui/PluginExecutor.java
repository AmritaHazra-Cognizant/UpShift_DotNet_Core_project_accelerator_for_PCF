package com.cognizant.blueprintwithui;

import com.google.gson.JsonObject;

public class PluginExecutor {

    /**
     * This method can be used to execute the functionality of this plugin.
     *
     * @param projectRootPath String
     *            - is the path to the input project given by the user.
     *            - if changes are made inside this folder and if the postprocessing is specified as "commit" in ui.json,
     *               then, all these changes will be commited back to github by UpShift.
     * @param pluginOutputFolderPath String
     *            - is the path where the plugin can write certain files/folders
     *               (as per the configurations made in StartUpFiles/ui.json file)
     * @param outputJson JsonObject
     *            - is the json output which will be displayed in results page as key value pairs, in a table.
     */
    public void execute(String projectRootPath, String pluginOutputFolderPath, JsonObject outputJson) {

        // TODO: Plugin functionality to be added

    }

}
