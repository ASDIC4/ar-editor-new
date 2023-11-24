// 编写 by 赵嘉诚
package com.example.dao;

import com.example.entity.Mapp;
import org.apache.ibatis.annotations.*;
import org.springframework.stereotype.Repository;
import tk.mybatis.mapper.common.Mapper;

import java.util.List;
import java.util.Map;

@Repository
public interface MapDao extends Mapper<Mapp> {
    @Insert("INSERT INTO maps (mapId, userId, easyarId, easyarName) " +
            "VALUES (#{mapp.mapId}, #{mapp.userId}," +
            "#{mapp.easyarId}, #{mapp.easyarName} )"
    )
    void insertMap(@Param("mapp") Mapp mapp);

    @Select("SELECT MAX(mapId) FROM maps")
    Integer getMaxMapId();
    @Select("SELECT * from maps where userId = #{userId}")
    List<Mapp> findBySearch(Integer userId);

    void deleteByMapId(@Param("mapId") Integer mapId);
}