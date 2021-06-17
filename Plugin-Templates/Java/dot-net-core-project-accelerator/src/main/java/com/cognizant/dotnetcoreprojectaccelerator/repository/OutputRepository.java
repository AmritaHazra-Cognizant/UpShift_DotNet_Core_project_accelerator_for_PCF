package com.cognizant.dotnetcoreprojectaccelerator.repository;

import java.util.List;
import java.util.Optional;

import com.cognizant.dotnetcoreprojectaccelerator.model.Output;

public interface OutputRepository {


    List<Output> findAll();

    Optional<Output> findById(String id);

    Output save(Output settings);

    void deleteById(String id);

}
