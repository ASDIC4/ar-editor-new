package com.example.service;

import com.example.dao.MapDao;
import com.example.entity.Mapp;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import java.util.List;

@Service
public class MapService {
    @Resource
    private MapDao mapDao;
    public void insertMap(Mapp mapp){
        mapDao.insertMap(mapp);
    }
    public Integer getMaxMapId(){
        return mapDao.getMaxMapId();
    }

    public List<Mapp> findBySearch(Integer userId){
        return mapDao.findBySearch(userId);
    }

    public void deleteByMapId(Integer mapId) {mapDao.deleteByMapId(mapId);}
}
