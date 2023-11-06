package com.example.service;

import com.example.common.JwtTokenUtils;
import com.example.dao.UserDao;
import com.example.entity.User;
import com.example.exception.CustomException;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
@Service
public class UserService {
    @Resource
    private UserDao userDao;
    public User login(User user) {
        System.out.println("user");
        System.out.println(user.getUserName());
        // 1. 进行一些非空判断
        if (user.getUserName() == null || "".equals(user.getUserName())) {
            throw new CustomException("用户名不能为空");
        }
        if (user.getPassword() == null || "".equals(user.getPassword())) {
            throw new CustomException("密码不能为空");
        }
        // 2. 从数据库里面根据这个用户名和密码去查询对应的管理员信息，
        User newUser = userDao.findByNameAndPassword(user.getUserName(), user.getPassword());
        if (newUser == null) {
            // 如果查出来没有，那说明输入的用户名或者密码有误，提示用户，不允许登录
            throw new CustomException("用户名或密码输入错误");
        }
        // 如果查出来了有，那说明确实有这个管理员，而且输入的用户名和密码都对；
        // 生成jwt token给前端
        String token = JwtTokenUtils.genToken(newUser.getUserId().toString(), newUser.getPassword());
        newUser.setToken(token);
        return newUser;
    }
    public User findById(Integer id){
        return userDao.selectByPrimaryKey(id);
    }
}
