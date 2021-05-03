package com.cognizant.customrecipe.model;

public class ErrorLog {

    private String errorText;

    private String errorMessage;

    public ErrorLog(String errorText, String errorMessage) {
        this.errorText = errorText;
        this.errorMessage = errorMessage;
    }

    public String getErrorText() {
        return errorText;
    }

    public void setErrorText(String errorText) {
        this.errorText = errorText;
    }

    public String getErrorMessage() {
        return errorMessage;
    }

    public void setErrorMessage(String errorMessage) {
        this.errorMessage = errorMessage;
    }
}
