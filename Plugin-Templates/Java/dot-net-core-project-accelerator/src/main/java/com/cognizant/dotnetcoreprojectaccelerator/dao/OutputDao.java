package com.cognizant.dotnetcoreprojectaccelerator.dao;

import java.util.List;

import com.cognizant.dotnetcoreprojectaccelerator.model.Output;

public interface OutputDao {

    public Output save(Output output);

    public List<Output> findAll();

    public Output findOne(String id);

    public void deleteById(String id);

}
