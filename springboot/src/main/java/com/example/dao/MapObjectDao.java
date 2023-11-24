// 编写 by 赵嘉诚
package com.example.dao;

import com.example.entity.MapObject;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface MapObjectDao {

    @Insert("INSERT INTO map_object (mapId, objectId, positionX, positionY, positionZ," +
            "rotationX, rotationY, rotationZ, scaleX, scaleY, scaleZ) " +
            "VALUES (#{mapObject.mapId}, #{mapObject.objectId}," +
            "#{mapObject.positionX}, #{mapObject.positionY}, #{mapObject.positionZ}," +
            "#{mapObject.rotationX}, #{mapObject.rotationY}, #{mapObject.rotationZ}," +
            "#{mapObject.scaleX}, #{mapObject.scaleY}, #{mapObject.scaleZ} )" )
    void insertMapObject(@Param("mapObject") MapObject mapObject);
}