FROM php:apache
#https://hub.docker.com/layers/library/php/8.3.3RC1-apache/images/sha256-52a52eeb2daf4c43cc73388015cda0480d6fe99b20e089f02e915d37098875e4?context=explore


RUN apt-get -y update \
&& apt-get install -y libicu-dev \
&& docker-php-ext-configure intl \
&& docker-php-ext-install intl
# Copy the Frontend files to the /var/www/html/ directory
COPY src/ /var/www/html/

RUN chmod -R 777 /var/www/html/system
RUN chmod -R 777 /var/www/html/writable

# Set the APACHE_DOCUMENT_ROOT environment variable as index.php ist the entrypoint
ENV APACHE_DOCUMENT_ROOT /var/www/html/public

RUN sed -ri -e 's!/var/www/html!${APACHE_DOCUMENT_ROOT}!g' /etc/apache2/sites-available/*.conf
RUN sed -ri -e 's!/var/www/!${APACHE_DOCUMENT_ROOT}!g' /etc/apache2/apache2.conf /etc/apache2/conf-available/*.conf

RUN sed -i '/<Directory \/var\/www\/>/,/<\/Directory>/ s/AllowOverride None/AllowOverride All/' /etc/apache2/apache2.conf