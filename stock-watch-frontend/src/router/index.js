import { createRouter, createWebHistory } from "vue-router";
import LoginView from "@/views/LoginView.vue";
//import DashboardView from "@/views/DashboardView.vue";
import StockUpdatesView from "@/views/StockUpdatesView.vue";
import { getToken } from "@/services/authService";
import ProductList from '../components/ProductList.vue';
import CreateProduct from '../components/CreateProduct.vue';
import UpdateProduct from '../components/UpdateProduct.vue';
import UpdateStock from '../components/UpdateStock.vue';
import ReportsStock from '../components/ReportsStock.vue';
import { isTokenExpired } from "../services/authService";


const routes = [
  { path: "/", component: LoginView },
  {
    path: "/dashboard",
    component: StockUpdatesView,
    meta: { requiresAuth: true },
  },
  {
    path: "/reports",
    component: ReportsStock,
    meta: { requiresAuth: true },
  },
  {
    path: '/productlist',
    name: 'product-list',
    component: ProductList,
  },
  {
    path: '/create',
    name: 'create-product',
    component: CreateProduct,
  },
  {
    path: '/update/:id',
    name: 'update-product',
    component: UpdateProduct,
  },
  {
    path: '/updatestock/:id',
    name: 'update-stock',
    component: UpdateStock,
  },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
  });

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !getToken()|| isTokenExpired()) {
    next("/");
  } else {
    next();
  }
});

export default router;
