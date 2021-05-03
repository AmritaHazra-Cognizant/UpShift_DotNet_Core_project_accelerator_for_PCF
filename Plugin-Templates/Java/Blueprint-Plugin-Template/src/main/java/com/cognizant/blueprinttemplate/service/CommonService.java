package com.cognizant.blueprinttemplate.service;

import com.google.gson.JsonObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.List;

import static com.cognizant.blueprinttemplate.constants.Constants.*;

@Service
public class CommonService {

    private Logger LOGGER = LoggerFactory.getLogger(CommonService.class);

    @Value("${upshiftUrl:http://localhost:8088}")
    private String upshiftUrl;

    @Value("${plugins.api.key}")
    private String pluginsApiKey;

    @Autowired
    private RestTemplate restTemplate;

    private String authorizationToken;

    /**
     * This method is used to get the Authorization token from UpShift (Orchestration Engine), by sending plugins API key
     *
     * @return String
     */
    private String getAuthorizationTokenFromOrchestrationEngine() {
        LOGGER.info("Getting Authorization token from UpShift (Orchestration Engine)");

        String getJwtTokenUrl = upshiftUrl.concat(UPSHIFT_ORCHESTRATION_PLUGINS_TOKEN_ENDPOINT);

        JsonObject requestBodyJson = new JsonObject();
        requestBodyJson.addProperty(API_KEY, pluginsApiKey);

        HttpEntity<String> requestBody = new HttpEntity<>(requestBodyJson.toString(), null);
        ResponseEntity<String> response = restTemplate.postForEntity( getJwtTokenUrl, requestBody, String.class );
        HttpHeaders headers = response.getHeaders();
        List<String> authorizationList = headers.get(AUTHORIZATION);
        if (authorizationList != null && authorizationList.size() > 0) {
            LOGGER.info("Got Authorization token from UpShift (Orchestration Engine)");
            return authorizationList.get(0);
        }
        LOGGER.warn("Failed to get Authorization token from UpShift (Orchestration Engine)");
        return null;
    }

    /**
     * This method is used to get the Authorization token
     *
     * @return String
     */
    public String getAuthorizationToken() {
        if (authorizationToken == null) {
            authorizationToken = getAuthorizationTokenFromOrchestrationEngine();
        }
        return authorizationToken;
    }

}
