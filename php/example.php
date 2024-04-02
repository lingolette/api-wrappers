<?php

require './lingolette.php';

$api = new Lingolette('YOUR_ORG_ID', 'YOUR_ORG_SECRET');

try
{
    $result = $api->call('org', 'createUserSession', array
    (
        'userId' => 'YOUR_ORG_USER_ID',
    ));
}
catch (\Exception $e)
{
    $result = "API error: {$e->getMessage()}\n";
}

print_r($result);
