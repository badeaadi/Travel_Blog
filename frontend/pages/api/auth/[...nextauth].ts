import NextAuth, { AuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import axios from 'axios';
import jwt from 'jsonwebtoken';
import { BASE_URL, User } from "../../../types";

export const authOptions: AuthOptions = {
  providers: [
    CredentialsProvider({
        name: "Auth",
        credentials: {
            email: {
                label: "Email",
                type: "text"
            },
            password: {
                label: "Password",
                type: "text"
            }
        },
        async authorize(credentials, req) {
            try {
                const res: any = await axios.post(`${BASE_URL}/api/User/login`, credentials);
                const userData = (jwt.decode(res.data.token) as User);
                return ({
                    ...userData,
                    jwt: res.data.token
                })
            } catch (error) {
                return null;
            }
        }
    })
  ],
  pages: {
    signIn: "/auth/login"
  }
}
export default NextAuth(authOptions)