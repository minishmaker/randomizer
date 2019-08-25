.thumb
ldrh	r0,[r6,#0x0C]
add	r0,#1
strh	r0,[r6,#0x0C]
ldr	r1,=#0x8055AE8
ldr	r1,[r1]
push	{r0-r7}
@check if the entry is the last in the list
ldr	r0,=#0x2002EB8
ldr	r1,table
ldrb	r2,[r0]
lsl	r2,#2
add	r1,r2
ldrh	r2,[r1,#2]
ldrb	r3,[r0,#1]
add	r3,#1
strb	r3,[r0,#1]
cmp	r2,r3
bhi	end
mov	r3,#0
strb	r3,[r0,#1]
ldrb	r2,[r0]
add	r2,#1
strb	r2,[r0]
add	r1,#4
ldrh	r2,[r1]
ldr	r3,=#0xFFFF
cmp	r2,r3
bne	end
mov	r2,#0
strb	r2,[r0]
ldr	r1,table
end:
@load the color
ldrh	r2,[r1]
ldr	r0,=#0x50001E2
ldr	r1,=#0x2017882
strh	r2,[r1]
@check if we are doing a fade
ldr	r1,=#0x20354FE
ldrh	r1,[r1]
cmp	r1,#0
bne	isfade
strh	r2,[r0]
isfade:
pop	{r0-r7}
ldr	r3,=#0x8055AC9
bx	r3
.align
.ltorg
table:
