package com.cognizant.blueprinttemplate.service;

import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

@Service
public class RestServices {

    private RestTemplate rest;

    public RestServices() {
        this.rest = new RestTemplate();
    }

    public ResponseEntity getCall(String uri, HttpHeaders headers) {
        HttpEntity<String> requestEntity = new HttpEntity<String>(headers);
        ResponseEntity<String> responseEntity = rest.exchange(uri, HttpMethod.GET, requestEntity, String.class);
        return responseEntity;
    }

    public ResponseEntity postCall(String uri, Object body, HttpHeaders headers) {
        HttpEntity<Object> requestEntity = new HttpEntity<Object>(body, headers);
        ResponseEntity<String> responseEntity = rest.exchange(uri, HttpMethod.POST, requestEntity, String.class);
        return responseEntity;
    }

    public ResponseEntity putCall(String uri, Object body, HttpHeaders headers) {
        HttpEntity<Object> requestEntity = new HttpEntity<Object>(body, headers);
        ResponseEntity<String> responseEntity = rest.exchange(uri, HttpMethod.PUT, requestEntity, String.class);
        return responseEntity;
    }

    public ResponseEntity<String> deleteCall(String uri, HttpHeaders headers) {
        HttpEntity<Object> requestEntity = new HttpEntity<>(headers);
        ResponseEntity<String> responseEntity = rest.exchange(uri, HttpMethod.DELETE, requestEntity, String.class);
        return responseEntity;
    }

}
