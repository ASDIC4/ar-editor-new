<template>
  <div>
    <div
      style="
        width: 400px;
        height: 350px;
        margin: 150px auto;
        background-color: rgba(107, 149, 224, 0.5);
        border-radius: 10px;
      "
    >
      <div
        style="
          width: 100%;
          height: 100px;
          font-size: 30px;
          line-height: 100px;
          text-align: center;
          color: #4a5ed0;
        "
      >
        欢迎登录
      </div>
      <div style="margin-top: 25px; text-align: center; height: 320px">
        <el-form :model="user">
          <el-form-item>
            <el-input
              v-model="user.userName"
              prefix-icon="el-icon-user"
              style="width: 80%"
              placeholder="请输入用户名"
            ></el-input>
          </el-form-item>
          <el-form-item>
            <el-input
              v-model="user.password"
              prefix-icon="el-icon-lock"
              style="width: 80%"
              placeholder="请输入密码"
            ></el-input>
          </el-form-item>
          <el-form-item>
            <el-button
              style="width: 80%; margin-top: 10px"
              type="primary"
              @click="login()"
              >登录</el-button
            >
          </el-form-item>
        </el-form>
      </div>
    </div>
  </div>
</template>

<style>
body,
h1,
h2,
h3,
h4,
h5,
p,
span,
div {
  font-family: "Arial", sans-serif;
}
</style>
<script>
import request from "@/utils/request";

export default {
  name: "Login",
  data() {
    return {
      user: sessionStorage.getItem("user") ? JSON.parse(sessionStorage.getItem("user")) : {},
    };
  },
  // 页面加载的时候，做一些事情，在created里面
  created() {},
  // 定义一些页面上控件出发的事件调用的方法
  methods: {
    login() {
      console.log("userName")
      console.log(this.user.userName);
      console.log(this.user);
      request.post("/login", this.user).then((res) => {
        if (res.code === "0") {
          this.$message({
            message: "登录成功",
            type: "success",
          });
          console.log(res.data);
          // 用户在登录成功后，需要将后端返回的token存储在localStorage中
          // 假设后端返回的token在res.data.token中
          // sessionStorage.setItem('token', res.data.token);
          sessionStorage.setItem("user", JSON.stringify(res.data));
         
          this.$router.push("/");
        } else {
          this.$message({
            message: res.msg,
            type: "error",
          });
        }
      });
    },
  },
};
</script>