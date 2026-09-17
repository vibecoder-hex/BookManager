import axios, { AxiosError } from "axios"
import type { IResponseOperationResult, IBookResponseBody, IBookRequestBody} from "../models/interfaces.ts"

const BASE_URL = "/api/BookManager"

class ErrorHandler {
    public static handleError(error: AxiosError | unknown): string
    {
        if (axios.isAxiosError(error)) {
            switch (error.response?.status) {
                case 400:
                    return "Bad request"
                case 401:
                    return "Unauthorized"
                case 403:
                    return "Forbidden"
                case 404:
                    return "Not Found"
                case 500:
                    return "Internal Server Error"
            }
        }
        return `${error}`
    }
}

export class BookManagerRequests  {
    private static getRequestObject(title: string, annotation: string, authorList: string[]): IBookRequestBody {
        return {
            name: title,
            annotation: annotation,
            authors: authorList.join(",")
        }
    }
    
    public static async addBook(title: string, annotation: string, authorList: string[]): Promise<IResponseOperationResult<IBookResponseBody>> {
        console.log(this.getRequestObject(title, annotation, authorList))
        try {
            const requestObject = this.getRequestObject(title, annotation, authorList);
            const request = await axios.post<IBookResponseBody>(BASE_URL, requestObject);
            return {
                operation: {
                    isValid: true,
                    errorMessage: null
                },
                responseData: request.data
            }
        } catch (error) {
            return {
                operation: {
                    isValid: false,
                    errorMessage: ErrorHandler.handleError(error)
                },
                responseData: null
            }
        }
    }
    
    public static async getBooks(): Promise<IResponseOperationResult<IBookResponseBody[]>> {
        try {
            const request = await axios.get<IBookResponseBody[]>(BASE_URL);
            return {
                operation: {
                    isValid: true,
                    errorMessage: null
                },
                responseData: request.data
            }
        } catch (error) {
            return {
                operation: {
                    isValid: false,
                    errorMessage: ErrorHandler.handleError(error)
                },
                responseData: null
            }
        }
    }
    
    public static async removeBook(title: string): Promise<IResponseOperationResult<null>> {
        try {
            await axios.delete(`${BASE_URL}`, {params: {title}});
            return {
                operation: {
                    isValid: true,
                    errorMessage: null
                },
                responseData: null
            }
        } catch (error) {
            return {
                operation: {
                    isValid: false,
                    errorMessage: ErrorHandler.handleError(error)
                },
                responseData: null
            }
        }
    }
    
    public static async searchBooksByFullText(searchQuery: string): Promise<IResponseOperationResult<IBookResponseBody[]>> {
        try {
            const response = await axios.get<IBookResponseBody[]>(`${BASE_URL}/FullText`, {params: {searchQuery}});
            return {
                operation: {
                    isValid: true,
                    errorMessage: null
                },
                responseData: response.data
            }
        } catch (error) {
            return {
                operation : {
                    isValid: false,
                    errorMessage: null
                },
                responseData: null
            }
        }
    }
    
    public static async searchBooksByRegex(regexString: string): Promise<IResponseOperationResult<IBookResponseBody[]>> {
        try {
            const response = await axios.get<IBookResponseBody[]>(`${BASE_URL}/Regex`, {params: {regexString}});
            return {
                operation: {
                    isValid: true,
                    errorMessage: null
                },
                responseData: response.data
            }
        } catch (error) {
            return {
                operation : {
                    isValid: false,
                    errorMessage: null
                },
                responseData: null
            }
        }
    }
}