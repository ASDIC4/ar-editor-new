package com.example.dao;

import com.example.entity.User;
import org.apache.ibatis.annotations.Param;
import org.apache.ibatis.annotations.Select;

public interface UserDao {
    @Select("select * from users where userName = #{userName} and password = #{password} limit 1")
    User findByNameAndPassword(@Param("userName") String userName, @Param("password") String password);

    @Select("select * from users where userId = #{userId} limit 1")
    User selectByPrimaryKey(@Param("userId") Integer userId);
}