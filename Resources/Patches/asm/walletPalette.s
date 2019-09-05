.thumb
pop	{r3}
add	r0,#0xA8
ldrb	r0,[r0]
lsl	r0,#2
push	{r0-r3}
ldr	r1,table
add	r1,r0
ldr	r2,=#0x50001E2
ldr	r3,=#0x2017882
ldrh	r0,[r1]
strh	r0,[r2]
strh	r0,[r3]
ldrh	r0,[r1,#2]
strh	r0,[r2,#2]
strh	r0,[r3,#2]
end:
pop	{r0-r3}
add	r0,r1
ldrh	r1,[r0,#2]
push	{r3}
ldrb	r3,=#0x801C554
mov	lr,r3
pop	{r3}
.short	0xF800
.align
.ltorg
table:
