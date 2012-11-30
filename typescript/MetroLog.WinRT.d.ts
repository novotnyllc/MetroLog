///<reference path='winrt.d.ts' static='true' />

// This is built-in to WinJS
class Debug {
    static writeln(message: string): void;
}    

module MetroLog {

    export module WinRT {

          export enum LogLevel {
                trace,
                debug,
                info,
                warn,
                error,
                fatal,
            }

        export interface ILogger {
            isEnabled(level: LogLevel): bool;

            info(message: string): void;
            info(message: string, ps: any[]): void;

            trace(message: string): void;
            trace(message: string, ps: any[]): void;

            debug(message: string): void;
            debug(message: string, ps: any[]): void;

            warn(message: string): void;
            warn(message: string, ps: any[]): void;

            error(message: string): void;
            error(message: string, ps: any[]): void;

            fatal(message: string): void;
            fatal(message: string, ps: any[]): void;

            log(level: LogLevel, message: string): void;
            log(level: LogLevel, message: string, ps: any[]): void;

            name: string;
            isTraceEnabled: bool;
            isDebugEnabled: bool;
            isInfoEnabled: bool;
            isWarnEnabled: bool;
            isErrorEnabled: bool;
            isFatalEnabled: bool;
        }



        export class Logger {
            static getLogger(name : string): ILogger;
            static getCompressedLogs(): Windows.Foundation.IAsyncOperation__StorageFile;
            static maxLevel: LogLevel;
            static addEventListener(eventName: string, f: Function): void;
            static removeEventListener(eventName: string);
        }
    }
}