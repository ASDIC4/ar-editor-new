<template>
  <div id="layout">
    <el-container>
      <el-header
        style="background-color: #84b4f8; display: flex; align-items: center; position:relative"
      >
        <img
          src="@/assets/arkit3.png"
          alt=""
          style="width: 40px; position: relative; cursor: pointer"
          @click="goToHomePage"
        />
        <span
          style="
            font-size: 20px;
            margin-left: 15px;
            color: white;
            font-family: 'Arial', sans-serif;
            cursor: pointer;
            vertical-align: middle;
          "
          @click="goToHomePage"
          >AR编辑器管理平台</span
        >

        <!-- <el-button @click="logout" style="margin-top: 10px; float: right"
          >退出登录</el-button
        > -->
        <!-- <el-dropdown style="float: right; height: 60px; line-height: 60px"> -->
        <el-dropdown style="float: right; margin-left: 200px; position: absolute; right: 0;">
          <span
            class="el-dropdown-link"
            style="
              color: white;
              font-size: 20px;
              margin-right: 15px;
              font-family: 'Arial', sans-serif;
            "
            >{{ user.username }}<i class="el-icon-arrow-down el-icon--right"></i
          ></span>
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item>
              <div @click="logout">退出登录</div>
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-header>
    </el-container>

    <el-container>
      <el-aside
        style="
          overflow: hidden;
          min-height: 100vh;
          background-color: #e4efff;
          width: 250px;
        "
      >
        <el-menu :default-active="$route.path" router>
          <el-menu-item index="/asset">
            <i class="el-icon-video-camera"></i>
            <span>流素材管理</span>
          </el-menu-item>

          <el-menu-item index="/scene">
            <i class="el-icon-menu"></i>
            <span slot="title">场景管理</span>
          </el-menu-item>

          <el-menu-item index="/other">
            <i class="el-icon-setting"></i>
            <span slot="title">其他</span>
          </el-menu-item>
        </el-menu>
      </el-aside>
      <el-main style="background-color: #ffffff">
        <router-view />
      </el-main>
    </el-container>
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
div,
el-button {
  font-family: "Arial", sans-serif;
}
</style>


<script>
export default {
  name: "Layout",
  data() {
    return {
      user: sessionStorage.getItem("user")
        ? JSON.parse(sessionStorage.getItem("user"))
        : {},
      submenuIndex: "1111",
    };
  },
  methods: {
    handleClose() {
      // 处理 close 事件的逻辑
    },
    logout() {
      sessionStorage.removeItem("user");
      sessionStorage.removeItem("token")
      if (this.$route.path !== "/login") {
        this.$router.push("/login"); // 只有在当前路由不是 "/" 时才进行导航
      }
    },
    goToHomePage() {
      if (this.$route.path !== "/") {
        this.$router.push("/"); // 只有在当前路由不是 "/" 时才进行导航
      }
    },
  },
};
</script>
