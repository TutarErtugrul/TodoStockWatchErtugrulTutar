<template>
  <nav>
    <span v-if="user">Hoş geldin, {{ user.username }}!</span>
    <button v-if="user" @click="handleLogout">Çıkış Yap</button>
    <router-link v-else to="/login">Giriş Yap</router-link>
  </nav>
</template>

<script>
import { ref, onMounted } from "vue";
import { getCurrentUser, logout } from "@/services/authService";

export default {
  setup() {
    const user = ref(null);

    onMounted(async () => {
      user.value = await getCurrentUser();
    });

    const handleLogout = () => {
      logout();
      window.location.href = "/login";
    };

    return { user, handleLogout };
  },
};
</script>
