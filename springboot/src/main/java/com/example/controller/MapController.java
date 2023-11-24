// 编写 by 赵嘉诚
package com.example.controller;

import com.example.common.Result;
import com.example.entity.MapObject;
import com.example.entity.Mapp;
import com.example.entity.PlaneObject;
import com.example.service.MapObjectService;
import com.example.service.MapService;
import org.apache.ibatis.annotations.Results;
import org.springframework.web.bind.annotation.*;

import javax.annotation.Resource;
import java.util.List;
import java.util.Map;

@CrossOrigin
@RestController
@RequestMapping("/scene")
public class MapController {

    @Resource
    private MapService mapService;
    @Resource
    private MapObjectService mapObjectService;

    @GetMapping("/getMapId/{userId}/{easyarId}/{easyarName}")
    public Result insertMap(@PathVariable Integer userId, @PathVariable String easyarId,
                             @PathVariable String easyarName){
        Integer newMapId = mapService.getMaxMapId() + 1;
        Mapp newMap = new Mapp(newMapId, userId, easyarId, easyarName);
        mapService.insertMap(newMap);
        return Result.success(newMapId);
    }

    @PostMapping("/upload/{mapId}/{objectId}/{positionX}/{positionY}/{positionZ}/{rotationX}/{rotationY}/{rotationZ}/{scaleX}/{scaleY}/{scaleZ}")
    public Result insertMapObject(@PathVariable Integer mapId, @PathVariable Integer objectId,
                                    @PathVariable Float positionX, @PathVariable Float positionY, @PathVariable Float positionZ,
                                    @PathVariable Float rotationX, @PathVariable Float rotationY, @PathVariable Float rotationZ,
                                    @PathVariable Float scaleX, @PathVariable Float scaleY, @PathVariable Float scaleZ) {
        MapObject newMapObject  = new MapObject(mapId, objectId, positionX, positionY, positionZ,
                rotationX, rotationY, rotationZ, scaleX, scaleY, scaleZ);

        mapObjectService.insertMapObject(newMapObject);
        return Result.success();
    }
    @GetMapping("/search/{userId}")
    public Result findBySearch(@PathVariable Integer userId){
        List<Mapp> list = mapService.findBySearch(userId);
        return Result.success(list);
    }
    @DeleteMapping("/{mapId}")
    public Result delete(@PathVariable Integer mapId){
        mapService.deleteByMapId(mapId);
        return Result.success();
    }
}
