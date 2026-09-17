export interface IBookResponseBody {
    name: string;
    authors: string;
    annotation: string;
    publishingDate: string;
}

export interface IBookRequestBody {
    name: string;
    authors: string;
    annotation: string;
}

export interface IOperationResultBody {
    isValid: boolean;
    errorMessage: string | null;
}

export interface IResponseOperationResult<T> {
    operation: IOperationResultBody;
    responseData: T | null;
}