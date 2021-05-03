package com.cognizant.blueprinttemplate.service.persistence;

import com.cognizant.blueprinttemplate.service.CommonService;
import com.cognizant.blueprinttemplate.service.RestServices;
import com.google.gson.JsonObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import static com.cognizant.blueprinttemplate.constants.Constants.AUTHORIZATION;

@Service
public class DataPersistenceService {

    private Logger LOGGER = LoggerFactory.getLogger(DataPersistenceService.class);

    @Value("${upshiftUrl:http://localhost:8088}")
    private String upshiftUrl;

    @Autowired
    RestServices restServices;

    @Autowired
    private CommonService commonService;

    private String PLUGINS_DATA_ENDPOINT_PREFIX = "/orchestration/plugins/data";
    private String KEY = "key";
    private String VALUE = "value";

    /**
     * Get an object (which is stored earlier)
     *
     * @param processInstanceId String
     * @param key String
     *
     * @return ResponseEntity
     */
    public ResponseEntity getPluginData(String processInstanceId, String key) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String getPluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_ENDPOINT_PREFIX)
                .concat("/")
                .concat(key);

        if (processInstanceId != null) {
            getPluginDataFileUrl = getPluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        return restServices.getCall(getPluginDataFileUrl, httpHeaders);
    }

    /**
     * Store an object
     *
     * @param processInstanceId String
     * @param key String
     * @param value String
     *
     * @return ResponseEntity
     */
    public ResponseEntity createPluginData(String processInstanceId, String key, String value) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String postPluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_ENDPOINT_PREFIX);

        if (processInstanceId != null) {
            postPluginDataFileUrl = postPluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty(KEY, key);
        jsonObject.addProperty(VALUE, value);

        return restServices.postCall(postPluginDataFileUrl, jsonObject.toString(), httpHeaders);

    }

    /**
     * Update an object (which is stored earlier)
     *
     * @param processInstanceId String
     * @param key String
     * @param value String
     *
     * @return ResponseEntity
     */
    public ResponseEntity updatePluginData(String processInstanceId, String key, String value) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String putPluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_ENDPOINT_PREFIX)
                .concat("/")
                .concat(key);

        if (processInstanceId != null) {
            putPluginDataFileUrl = putPluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        return restServices.putCall(putPluginDataFileUrl, value, httpHeaders);
    }

    /**
     * Delete an object (which is stored earlier)
     *
     * @param processInstanceId String
     * @param key String
     *
     * @return ResponseEntity
     */
    public ResponseEntity deletePluginData(String processInstanceId, String key) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String deletePluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_ENDPOINT_PREFIX)
                .concat("/")
                .concat(key);

        if (processInstanceId != null) {
            deletePluginDataFileUrl = deletePluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        return restServices.deleteCall(deletePluginDataFileUrl, httpHeaders);
    }

}
