#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <err.h>
#include <string.h>
#include<fcntl.h>
#include "http_parser.h"

char response[] = "HTTP/1.1 404 FILE NOT FOUND\r \n"
    "Content-Type: text/html; charset=UTF8\r\n\r\n"
    "<!DOCTYPE html><html><head><title>File Not Found</title>"
    "</head><body>Sorry, that file is not here</body></html>\r\n";

int one = 1, client_fd, sock;
struct sockaddr_in svr_addr, cli_addr;
socklen_t sin_len = sizeof(cli_addr);
char* data;
#define BYTES 1024
char mesg[99999], *reqline[3], data_to_send[BYTES], path[99999];
    int rcvd, fd, bytes_read;

/*
//typedef int (*my_url_callback)(http_parser *p, const char *buf, size_t len)
int my_url_callback(http_parser *p, const char *buf, size_t len){
    printf("url callback");
    return 0;
};

//typedef int (*my_header_field_callback) (http_parser*, const char *at, size_t length);
int my_header_field_callback(http_parser *p, const char *at, size_t length) {
    printf("header callback");
    return 0;
};

//typedef int (*headers_complete_cb) (http_parser *p);
int headers_complete_cb (http_parser *p) {
     printf("header comeplete");
    return 0;   
}
*/
http_parser_settings settings;
http_parser *parser;
        
int setupHTTP(void) {
    sock = socket(AF_INET, SOCK_STREAM, 0);
    
    if (sock < 0) {
        err(1, "can't open socket");
        return -1;
    }
 
    data = malloc(256);
    
    setsockopt(sock, SOL_SOCKET, SO_REUSEADDR, &one, sizeof(int));
 
    int port = 1807;
    svr_addr.sin_family = AF_INET;
    svr_addr.sin_addr.s_addr = INADDR_ANY;
    svr_addr.sin_port = htons(port);
 
    if (bind(sock, (struct sockaddr *) &svr_addr, sizeof(svr_addr)) == -1) {
        close(sock);
        err(1, "Can't bind");
        return -1;
    }
    
    http_parser_settings settings;
    //settings.on_url = my_url_callback;
    //settings.on_header_field = my_header_field_callback;
    //settings.on_headers_complete = headers_complete_cb;

    //    http_parser *parser = malloc(sizeof(http_parser));
    parser = malloc(sizeof(http_parser));
    http_parser_init(parser, HTTP_REQUEST);
    parser->data = sock;
    
    return 0;
};

int startHTTP(void) {
    listen(sock, 5);
    
    return 0;
}

int acceptHTTP(void) {
    client_fd = accept(sock, (struct sockaddr *) &cli_addr, &sin_len);
 
    if (client_fd == -1) {
      perror("Can't accept");
    }
    
 //////////////////   
    size_t len = 80*1024, nparsed;
char buf[len];
ssize_t recved;

recved = recv(client_fd, buf, len, 0);

if (recved < 0) {
  /* Handle error. */
}

/* Start up / continue the parser.
 * Note we pass recved==0 to signal that EOF has been received.
 */
nparsed = http_parser_execute(parser, &settings, buf, recved);

if (parser->upgrade) {
    printf("paser upgrade");    
  /* handle new protocol */
} else if (nparsed != recved) {
  /* Handle error. Usually just close the connection. */
        printf("paser error");
} else {   

    char *token;
    char *token2;
    token=strtok(buf,"\r");

    if ( strncmp(token, "GET", 3)==0 )
    {
        token2 = strtok(token," ");  
        token2 = strtok(NULL," ");        

            //if ( (fd=open("/Users/dlockamy/Documents/GitHub/Silver-Iodide/build/agid/webroot/index.html", O_RDONLY))!=-1 )
            //{
        printf("test opening:\n");
                fd=open("/Users/dlockamy/Documents/GitHub/Silver-Iodide/build/agid/webroot/index.html", O_RDONLY);
        printf(">> %i\n", fd);

    //printf("an error: %s\n", strerror(errno));
    //exit(1);
        
                    send(client_fd, "HTTP/1.0 200 OK\n\n", 17, 0);
        while ( (bytes_read=read(fd, data_to_send, BYTES))>0 ){
                 printf("bytes_read=%d\n",bytes_read);
                        send (client_fd, data_to_send, bytes_read,0);
            
        }
                return 0;
            //}
            //else    
                write(client_fd, "HTTP/1.0 404 Not Found\n", 23); //FILE NOT FOUND
        
        return 0;
        
        
        
        if ( strcmp(token, "/index.html")==0 )
        { 
            if ( (fd=open("/Users/dlockamy/Documents/GitHub/Silver-Iodide/build/agid/webroot/index.html", O_RDONLY))!=-1 )
            {
                    send(client_fd, "HTTP/1.0 200 OK\n\n", 17, 0);
                while ( (bytes_read=read(fd, data_to_send, BYTES))>0 ) {
                        write (client_fd, data_to_send, bytes_read);
                }
            }
            else    
                write(client_fd, "HTTP/1.0 404 Not Found\n", 23); //FILE NOT FOUND

            close(client_fd);           
        } else if ( strcmp(token, "/bin/bin.js")==0 )
        { 
            write(client_fd, response, sizeof(response) - 1); /*-1:'\0'*/
            close(client_fd);           
        } else if ( strcmp(token, "/lib/agi.lib.js")==0 )
        { 
            write(client_fd, response, sizeof(response) - 1); /*-1:'\0'*/
            close(client_fd);           
        }else if ( strcmp(token, "/lib/strophe.js")==0 )
        { 
            write(client_fd, response, sizeof(response) - 1); /*-1:'\0'*/
            close(client_fd);           
        }else if ( strcmp(token, "/main.js")==0 )
        { 
            write(client_fd, response, sizeof(response) - 1); /*-1:'\0'*/
            close(client_fd);           
        }else {
            printf("Requested file not found");
            write(client_fd, response, sizeof(response) - 1); /*-1:'\0'*/
            close(client_fd);             
        }
        return 0;
        
    } else if ( strncmp(token, "POST", 4)==0 )
    {
        printf("guess you're posting some stuff");
        return 0;
    } else
         printf("Unsupported HTTP request %s \n\n",token);    
}
    return 0;   
}