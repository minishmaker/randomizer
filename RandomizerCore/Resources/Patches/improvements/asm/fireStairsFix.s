.thumb
push	{r0-r2}
ldr	r0,=#0x3003F80
ldr	r1,[r0,#0x30]
ldr	r2,=#0x400
mvn	r2,r2
and	r1,r2
str	r1,[r0,#0x30]
pop	{r0-r2}

pop	{r3}
mov	r1, #0x1D
strb	r1, [r2, #0x0C]
strb	r0, [r2, #0x0D]
strh	r0, [r2, #0x30]
ldr	r0, =#0x807A6C6
mov	lr, r0
.short	0xF800