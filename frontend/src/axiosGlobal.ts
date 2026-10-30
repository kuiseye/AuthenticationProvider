import axios from "axios";

const globalAxios = axios.create({baseURL: "/api", withCredentials: true});

export default globalAxios