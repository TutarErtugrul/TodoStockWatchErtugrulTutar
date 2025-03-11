<template>
  <div class="login-container">
    <div class="login-box">
      <h2>Giriş Yap</h2>
      <form @submit.prevent="handleLogin">
        <div class="input-group">
          <label>Kullanıcı Adı</label>
          <input v-model="username" type="text" placeholder="Kullanıcı adınızı girin" required />
        </div>

        <div class="input-group">
          <label>Şifre</label>
          <input v-model="password" type="password" placeholder="Şifrenizi girin" required />
        </div>

        <button type="submit" :disabled="loading">
          <span v-if="loading">Giriş Yapılıyor...</span>
          <span v-else>Giriş Yap</span>
        </button>

        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </form>
    </div>
  </div>
</template>

<script>
import { ref } from "vue";
import { login } from "@/services/authService";

export default {
  setup() {
    const username = ref("");
    const password = ref("");
    const errorMessage = ref("");
    const loading = ref(false);

    const handleLogin = async () => {
      try {
        loading.value = true;
        await login(username.value, password.value);
        window.location.href = "/dashboard"; 
      } catch (error) {
        errorMessage.value = "Giriş başarısız! Kullanıcı adı veya şifre hatalı.";
      } finally {
        loading.value = false;
      }
    };

    return { username, password, handleLogin, errorMessage, loading };
  },
};
</script>

<style scoped>
/* Genel Sayfa Stili */
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: linear-gradient(135deg, #667eea, #764ba2);
}

/* Kart Görünümü */
.login-box {
  background: white;
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
  width: 350px;
  text-align: center;
}

/* Başlık */
h2 {
  margin-bottom: 20px;
  color: #333;
}

/* Input Grupları */
.input-group {
  margin-bottom: 15px;
  text-align: left;
}

.input-group label {
  display: block;
  font-size: 14px;
  font-weight: bold;
  color: #333;
  margin-bottom: 5px;
}

.input-group input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 16px;
}

/* Buton */
button {
  width: 100%;
  padding: 10px;
  background: #667eea;
  color: white;
  font-size: 16px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: 0.3s;
}

button:hover {
  background: #5a67d8;
}

button:disabled {
  background: gray;
  cursor: not-allowed;
}

/* Hata Mesajı */
.error {
  color: red;
  font-size: 14px;
  margin-top: 10px;
}
</style>
