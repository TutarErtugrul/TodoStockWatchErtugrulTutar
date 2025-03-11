<template>
  <div>
    <h2>Stok Raporları</h2>
    
    <div>
      <label>Başlangıç Tarihi:</label>
      <input type="date" v-model="start" />
      
      <label>Bitiş Tarihi:</label>
      <input type="date" v-model="end" />

      <button @click="fetchReports">Raporları Getir</button>
    </div>

    <h3>Toplam Satış Raporu</h3>
    <table>
      <thead>
        <tr>
          <th>Ürün id</th>
          <th>Ürün Adı</th>
          <th>Toplam Satılan Miktar</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in sumReport" :key="product.id">
          <td>{{ product.id }}</td>
          <td>{{ product.name }}</td>
          <td>{{ product.result }}</td>
        </tr>
      </tbody>
    </table>

    <h3>Stok Devir Hızı Raporu</h3>
    <table>
      <thead>
        <tr>
          <th>Ürün id</th>
          <th>Ürün Adı</th>
          <th>Stok Devir Hızı</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in avgReport" :key="product.id">
          <td>{{ product.id }}</td>
          <td>{{ product.name }}</td>
          <td>{{ product.result }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      start: "",
      end: "",
      sumReport: [],
      avgReport: []
    };
  },
  methods: {
    async fetchReports() {
      if (!this.start || !this.end) {
        alert("Lütfen başlangıç ve bitiş tarihlerini seçiniz.");
        return;
      }

      try {
        const sumResponse = await axios.get(`https://localhost:7079/api/products/GetSumReport`, {
          params: { start: this.start, end: this.end }
        });
        this.sumReport = sumResponse.data;

        const avgResponse = await axios.get(`https://localhost:7079/api/products/GetAvgReport`, {
          params: { start: this.start, end: this.end }
        });
        this.avgReport = avgResponse.data;

      } catch (error) {
        console.error("Raporlar alınırken hata oluştu:", error);
      }
    }
  }
};
</script>

<style scoped>
table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}
th, td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}
th {
  background-color: #f4f4f4;
}
</style>
