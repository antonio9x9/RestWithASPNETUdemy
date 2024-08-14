import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:10579",
});

export default api;
