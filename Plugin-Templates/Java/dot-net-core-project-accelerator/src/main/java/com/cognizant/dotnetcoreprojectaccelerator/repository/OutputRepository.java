package com.cognizant.dotnetcoreprojectaccelerator.repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.cognizant.dotnetcoreprojectaccelerator.model.Output;

public interface OutputRepository extends JpaRepository<Output, String>{


    List<Output> findAll();

    Optional<Output> findById(String id);

    Output save(Output settings);

    void deleteById(String id);

}
