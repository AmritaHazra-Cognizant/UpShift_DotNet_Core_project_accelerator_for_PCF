package com.cognizant.blueprinttemplate.dao;

import com.cognizant.blueprinttemplate.model.Output;

import java.util.List;

public interface OutputDao {

    public Output save(Output output);

    public List<Output> findAll();

    public Output findOne(String id);

    public void deleteById(String id);

}
