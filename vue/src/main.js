import Vue from 'vue'
import App from './App.vue'
import router from './router'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import '@/assets/global.css'

// 页面关闭前清除存储的用户信息
window.addEventListener('beforeunload', function (event) {
  // 清除存储的用户信息
  sessionStorage.removeItem('user');
  // sessionStorage.removeItem('token');
});

Vue.config.productionTip = false

Vue.use(ElementUI, { size: "small" });

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
