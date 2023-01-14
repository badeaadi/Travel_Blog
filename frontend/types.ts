export const BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/identity';

export interface UserCreate {
    email: string,
    password: string,
    firstName: string,
    lastName: string
}

export interface User {
    email: string,
    firstName: string,
    lastName: string,
    id: string
}