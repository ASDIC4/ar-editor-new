package com.example.controller;

import com.example.common.Result;
import com.example.dao.UserDao;
import com.example.entity.User;
import com.example.exception.CustomException;
import com.example.service.UserService;
import org.springframework.web.bind.annotation.*;

import javax.annotation.Resource;

@CrossOrigin
@RestController
public class UserController {
    @Resource
    private UserService userService;
    @PostMapping("/login")
    public Result login(@RequestBody User user) {
        User loginUser = userService.login(user);
        return Result.success(loginUser);
    }
}