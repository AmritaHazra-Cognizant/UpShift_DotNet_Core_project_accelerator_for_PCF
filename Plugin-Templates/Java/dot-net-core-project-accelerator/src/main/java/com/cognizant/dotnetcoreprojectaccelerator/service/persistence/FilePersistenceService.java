package com.cognizant.dotnetcoreprojectaccelerator.service.persistence;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.io.FileSystemResource;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.util.LinkedMultiValueMap;

import com.cognizant.dotnetcoreprojectaccelerator.service.CommonService;
import com.cognizant.dotnetcoreprojectaccelerator.service.RestServices;

import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.AUTHORIZATION;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

@Service
public class FilePersistenceService {

    private Logger LOGGER = LoggerFactory.getLogger(FilePersistenceService.class);

    @Value("${upshiftUrl:http://localhost:8088}")
    private String upshiftUrl;

    @Autowired
    RestServices restServices;

    @Autowired
    private CommonService commonService;

    private String PLUGINS_DATA_FILE_ENDPOINT_PREFIX = "/orchestration/plugins/data/file";
    private String FORM_DATA_FILE_KEY = "file";
    private String FORM_DATA_OBJECT_NAME_KEY = "objectName";

    /**
     * Get a file object (which is stored earlier)
     *
     * @param objectName String
     * @param processInstanceId String
     *
     * @return ResponseEntity
     */
    public ResponseEntity getPluginDataFile(String objectName, String processInstanceId) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);
        List<MediaType> acceptableMediaTypes = new ArrayList<>();
        acceptableMediaTypes.add(MediaType.ALL);
		httpHeaders.setAccept(acceptableMediaTypes );
		httpHeaders.setContentType(MediaType.APPLICATION_OCTET_STREAM);

        String getPluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_FILE_ENDPOINT_PREFIX)
                .concat("/")
                .concat(objectName);

        if (processInstanceId != null) {
            getPluginDataFileUrl = getPluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }


        return restServices.getCallForBlob(getPluginDataFileUrl, httpHeaders);
    }

    /**
     * Create a file object
     *
     * @param objectName String
     * @param processInstanceId String
     * @param file File
     *
     * @return ResponseEntity
     */
    public ResponseEntity createPluginDataFile(String objectName, String processInstanceId, File file) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String postPluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_FILE_ENDPOINT_PREFIX);

        if (processInstanceId != null) {
            postPluginDataFileUrl = postPluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        LinkedMultiValueMap<String, Object> map = new LinkedMultiValueMap<>();
        map.add(FORM_DATA_FILE_KEY, new FileSystemResource(file));
        map.add(FORM_DATA_OBJECT_NAME_KEY, objectName);

        return restServices.postCall(postPluginDataFileUrl, map, httpHeaders);

    }

    /**
     * Update a file object (which is stored earlier)
     *
     * @param objectName String
     * @param processInstanceId String
     * @param file File
     *
     * @return ResponseEntity
     */
    public ResponseEntity updatePluginDataFile(String objectName, String processInstanceId, File file) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String putPluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_FILE_ENDPOINT_PREFIX)
                .concat("/")
                .concat(objectName);

        if (processInstanceId != null) {
            putPluginDataFileUrl = putPluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        LinkedMultiValueMap<String, Object> map = new LinkedMultiValueMap<>();
        map.add(FORM_DATA_FILE_KEY, new FileSystemResource(file));

        return restServices.putCall(putPluginDataFileUrl, map, httpHeaders);
    }

    /**
     * Delete a file object (which is stored earlier)
     *
     * @param objectName String
     * @param processInstanceId String
     *
     * @return ResponseEntity
     */
    public ResponseEntity deletePluginDataFile(String objectName, String processInstanceId) {

        String authorizationToken = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationToken);

        String deletePluginDataFileUrl = upshiftUrl
                .concat(PLUGINS_DATA_FILE_ENDPOINT_PREFIX)
                .concat("/")
                .concat(objectName);

        if (processInstanceId != null) {
            deletePluginDataFileUrl = deletePluginDataFileUrl
                    .concat("?processInstanceId=").concat(processInstanceId);
        }

        return restServices.deleteCall(deletePluginDataFileUrl, httpHeaders);
    }


}
