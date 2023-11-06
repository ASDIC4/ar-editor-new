import Vue from 'vue'
import VueRouter from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/',
    component: () => import('../Layout.vue'),
    children: [ // 子路由
      {
        path: '/',
        name: 'home',
        component: HomeView
      },
      {
        path: '/asset',
        name: 'asset',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import('../views/AssetView.vue')
      },
      {
        path: '/scene',
        name: 'scene',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import('../views/SceneView.vue')
      }
    ]
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})
router.beforeEach((to, from, next) => {
  if (to.path === '/login') {
    next();
  }
  const user = sessionStorage.getItem("user");
  if (!user && to.path !== '/login') {
    return next("/login");
  }
  next();
})

export default router
