package com.cognizant.blueprinttemplate.model;

import javax.persistence.Entity;
import javax.persistence.Id;

@Entity
public class Output {

    @Id
    private String processInstanceId;

    private String projectName;

    private String status;

    public Output() { }

    public Output(String processInstanceId) {
        this.processInstanceId = processInstanceId;
    }

    public Output(String processInstanceId, String projectName) {
        this.processInstanceId = processInstanceId;
        this.projectName = projectName;
    }

    public Output(String processInstanceId, String projectName, String status) {
        this.processInstanceId = processInstanceId;
        this.projectName = projectName;
        this.status = status;
    }

    // TODO: Can add more columns, if needed

    public String getProcessInstanceId() { return processInstanceId; }

    public void setProcessInstanceId(String processInstanceId) { this.processInstanceId = processInstanceId; }

    public String getProjectName() { return projectName; }

    public void setProjectName(String projectName) { this.projectName = projectName; }

    public String getStatus() { return status; }

    public void setStatus(String status) { this.status = status; }

    @Override
    public String toString() {
        return "Output{" +
                "processInstanceId='" + processInstanceId + '\'' +
                "projectName='" + projectName + '\'' +
                "status='" + status + '\'' +
                '}';
    }
}
