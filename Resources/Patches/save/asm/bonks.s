.thumb
pop	{r3}
push	{r0-r7}
ldr	r1,offset
ldrh	r2,[r1]
ldr	r3,=#0xFFFF
cmp	r2,r3
beq	end
add	r2,#1
strh	r2,[r1]
end:
pop	{r0-r7}
mov	r0,#6
strb	r0,[r5,#0x0C]
strb	r2,[r5,#0x0D]
ldrb	r1,[r5,#0x10]
mov	r0,#0x7F
push	{r3}
ldr	r3,=#0x807651C
mov	lr,r3
pop	{r3}
.short	0xF800
.align
.ltorg
offset:
