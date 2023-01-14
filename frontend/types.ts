export const AUTH_BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/identity';

export const FEED_BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/blog_manager';

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

export interface FeedData {
    _ts: number;
    content: string;
    id: string;
    photo: string;
    summary: string;
    title: string;
    user: string;
}