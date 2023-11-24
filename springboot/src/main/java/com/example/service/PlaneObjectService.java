package com.example.service;

import com.example.dao.AssetDao;
import com.example.dao.PlaneObjectDao;
import com.example.entity.PlaneObject;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
@Service
public class PlaneObjectService {
    @Resource
    private PlaneObjectDao planeObjectDao;
    public void insertPlaneObject(PlaneObject planeObject) {
        planeObjectDao.insertPlaneObject(planeObject);
    }
}
