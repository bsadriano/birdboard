import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { router } from "../Routes/Routes";
import { PaginatedResponse } from "../Models/Pagination";
import { UserProfileToken } from "../Models/User";
// import { store } from "../store/configureStore";

const sleep = () => new Promise((resolve) => setTimeout(resolve, 500));

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.withCredentials = true;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    if (process.env.NODE_ENV === "development") await sleep();

    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = new PaginatedResponse(
        response.data,
        JSON.parse(pagination)
      );
      return response;
    }
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
      case 400:
        if (data.errors) {
          const modelStateErrors: string[] = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modelStateErrors.push(data.errors[key]);
            }
          }
          throw modelStateErrors.flat();
        }
        toast.error(data.title);
        break;
      case 401:
        toast.error(data.title);
        break;
      case 500:
        router.navigate("/server-error", { state: { error: data } });
        break;
      default:
        break;
    }
    return Promise.reject(error.response);
  }
);

const requests = {
  get: (url: string, params?: URLSearchParams) =>
    axios.get(url, { params }).then(responseBody),
  post: <T>(url: string, body: object) =>
    axios.post(url, body).then(responseBody<T>),
  put: (url: string, body: object) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
};

// const Catalog = {
//   list: (params: URLSearchParams) => requests.get("products", params),
//   details: (id: number) => requests.get(`products/${id}`),
//   fetchFilters: () => requests.get("products/filters"),
// };

// const TestErrors = {
//   get400Error: () => requests.get("buggy/bad-request"),
//   get401Error: () => requests.get("buggy/unauthorised"),
//   get404Error: () => requests.get("buggy/not-found"),
//   get500Error: () => requests.get("buggy/server-error"),
//   getValidationError: () => requests.get("buggy/validation-error"),
// };

// const Basket = {
//   get: () => requests.get("basket"),
//   addItem: (productId: number, quantity = 1) =>
//     requests.post(`basket?productId=${productId}&quantity=${quantity}`, {}),
//   removeItem: (productId: number, quantity = 1) =>
//     requests.del(`basket?productId=${productId}&quantity=${quantity}`),
// };

const Auth = {
  login: (values: { username: string; password: string }) =>
    requests.post<UserProfileToken>("auth/login", values),
  register: (values: any) =>
    requests.post<UserProfileToken>("auth/register", values),
  refreshToken: (values: any) => requests.post("auth/refresh-token", values),
  currentUser: () => requests.get("auth/currentUser"),
  fetchAddress: () => requests.get("auth/savedAddress"),
};

// const Orders = {
//   list: () => requests.get("orders"),
//   fetch: (id: number) => requests.get(`orders/${id}`),
//   create: (values: any) => requests.post("orders", values),
// };

// const Payments = {
//   createPaymentIntent: () => requests.post("payments", {}),
// };

const Agent = {
  Auth,
  // Catalog,
  // TestErrors,
  // Basket,
  // Orders,
  // Payments,
};

export default Agent;
