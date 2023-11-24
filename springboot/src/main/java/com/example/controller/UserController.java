// 编写 by 赵嘉诚
package com.example.controller;

import com.example.common.Result;
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
    @PostMapping("/login/{userName}/{password}")
    public Result login(@PathVariable String userName, @PathVariable String password) {
        User inputUser = new User();
        inputUser.setUserName(userName);
        inputUser.setPassword(password);
        User loginUser = userService.login(inputUser);
        return Result.success(loginUser);
    }
}