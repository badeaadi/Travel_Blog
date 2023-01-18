export const AUTH_BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/identity';

export const FEED_BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/blog_manager';

export const CHAT_BASE_URL = 'http://traver-blog-nst-srv-dns.northeurope.cloudapp.azure.com/direct-messaging';

export interface UserCreate {
    email: string,
    password: string,
    firstName: string,
    lastName: string,
    thumbnailUrl: string
}

export interface UserToken {
    sub: any;
    email: string,
    id: string,
    jwt: string
}

export interface ChatMessage {
    content: string,
    createdAt: Date,
    senderId: string,
    receiverId: string
}

export interface UserData extends UserToken {
    firstName: string,
    lastName: string,
    thumbnailUrl: string
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