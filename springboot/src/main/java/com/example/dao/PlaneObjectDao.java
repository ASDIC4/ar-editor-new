// 编写 by 赵嘉诚
package com.example.dao;

import com.example.entity.Asset;
import com.example.entity.PlaneObject;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Options;
import org.apache.ibatis.annotations.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface PlaneObjectDao {

    @Insert("INSERT INTO plane_object (assetId, objectId, positionX, positionY, positionZ," +
            "rotationX, rotationY, rotationZ, scaleX, scaleY, scaleZ) " +
            "VALUES (#{planeObject.assetId}, #{planeObject.objectId}," +
            "#{planeObject.positionX}, #{planeObject.positionY}, #{planeObject.positionZ}," +
            "#{planeObject.rotationX}, #{planeObject.rotationY}, #{planeObject.rotationZ}," +
            "#{planeObject.scaleX}, #{planeObject.scaleY}, #{planeObject.scaleZ} )" )
    void insertPlaneObject(@Param("planeObject") PlaneObject planeObject);
}
