// 编写 by 赵嘉诚
package com.example.service;

import com.example.dao.MapObjectDao;
import com.example.dao.PlaneObjectDao;
import com.example.entity.MapObject;
import com.example.entity.PlaneObject;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;

@Service
public class MapObjectService {
    @Resource
    private MapObjectDao mapObjectDao;
    public void insertMapObject(MapObject mapObject) {
        mapObjectDao.insertMapObject(mapObject);
    }
}