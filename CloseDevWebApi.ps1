    echo "======View 7000 point======"
    $points=(netstat -ano | findstr 7000)
    echo $points
    if($points -eq $null) {} else {
     if($points.gettype().Name -eq "Object[]"){$points=$points.get(0);}
     echo "======kill 7000 point======"
     $pids=-Split $points
     if($pids.length -gt 0) {taskkill /t /f /im $pids[$pids.length-1];echo "======kill 7000 success======";}
    }