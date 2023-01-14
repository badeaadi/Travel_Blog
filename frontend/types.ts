export const BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/identity';

export interface UserCreate {
    email: string,
    password: string,
    firstName: string,
    lastName: string
}

export interface UserToken {
    email: string,
    id: string,
    jwt: string
}

export interface UserData extends UserToken {
    firstName: string,
    lastName: string
}