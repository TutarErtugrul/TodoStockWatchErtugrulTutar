<template>
  <div>
    <h2>Stok Yönetimi</h2>
    <p>Burada stokları görebilirsiniz.</p>
    <button @click="handleLogout">Çıkış Yap</button>
  </div>
</template>

<script>
import { logout } from "@/services/authService";

export default {
  setup() {
    const handleLogout = () => {
      logout();
      window.location.href = "/login";
    };

    return { handleLogout };
  },
};
</script>
